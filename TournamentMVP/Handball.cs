using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;

namespace TournamentMVP
{
    public class Handball : ISport
    {
        public Players Calculate(List<string> matchData)
        {
            // Her pozisyon için derecelendirme puanları
            var positionPoints = new Dictionary<string, Tuple<int, int, int>>
                {
                    {"G", new Tuple<int, int, int>(50, 5, -2)},
                    {"F", new Tuple<int, int, int>(20, 1, -1)},
                };

            // Oyuncular
            var players = new Dictionary<string, Players>();

            // Maç verileri ve oyuncu için dereceye göre puanlama
            foreach (var data in matchData)
            {
                var fields = data.Split(';');
                var name = fields[0];
                var nickname = fields[1];
                var number = int.Parse(fields[2]);
                var teamName = fields[3];
                var position = fields[4];
                var goalsMade = int.Parse(fields[5]);
                var goalsReceived = int.Parse(fields[6]);


                // Sağlanan bilgilere dayanarak her oyuncunun derecelendirme puanları aşağıdaki formül kullanılarak hesaplanabilir:
                // İlk değerlendirme puanları + (atılan goller * atılan goller faktörü) - (alınan goller * alınan goller faktörü)

                // Örnekte verilen verileri kullanarak,
                // 1.oyuncu(A takımında bir kaleci) 35 reyting puanı(50 + 05 - 202 = 35),
                // 2.oyuncu(A takımında bir saha oyuncusu) 35 reyting puanı alacaktır. (20 + 151 - 201 = 35)
                // 4.oyuncu(B takımında bir kaleci) 32 puan alır(50 + 15 - 252 = 32).

                //  Hentbol maçının galibi daha fazla gol atan takımdır. Bu durumda A Takımı kazanır çünkü 25 gol(2.oyuncudan 15 + 3.oyuncudan 10) ve B Takımı 21 gol(12 oyuncu 5 + 8'den) atmıştır. oyuncu 6'dan).


                int initialPoints = positionPoints[position].Item1;
                int goalPoints = goalsMade * positionPoints[position].Item2;
                int receivedPoints = goalsReceived * positionPoints[position].Item3;

                var points = initialPoints + (goalPoints) - (receivedPoints);

                if (!players.ContainsKey(nickname))
                {
                    players.Add(nickname, new Players
                    {
                        Name = name,
                        Nickname = nickname,
                        Number = number,
                        TeamName = teamName,
                        Position = position,
                        RatingPoints = points
                    });
                }
                else
                {
                    players[nickname].RatingPoints += points;
                }


            }


            // En çok puana sahip oyuncuyu bul
            var mvp = players.Values.OrderByDescending(p => p.RatingPoints).First();

            // Her takım için atılan toplam golleri hesapla
            var teamScores = matchData.GroupBy(p => p.Split(';')[3])
                                  .Select(g => new
                                  {
                                      Team = g.Key,
                                      GoalsMade = g.Sum(p => int.Parse(p.Split(';')[5]))
                                  });

            var winner = teamScores.OrderByDescending(t => t.GoalsMade).First();

            // Oyuncunun takımı maçı kazanırsa 35 puan ekle
            if (mvp.TeamName == winner.Team)
            {
                mvp.RatingPoints += 35;
            }

            mvp.WinningTeam = winner.Team;
            mvp.TeamScore = winner.GoalsMade;

            return mvp;

        }


    }
}

