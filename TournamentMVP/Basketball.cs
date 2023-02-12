using System.Collections.Generic;
using System;
using System.Linq;

namespace TournamentMVP
{
    public class Basketball : ISport
    {
        public Players Calculate(List<string> matchData)
        {
            // Her pozisyon için derecelendirme puanları
            var positionPoints = new Dictionary<string, Tuple<int, int, int>>
                {
                    {"G", new Tuple<int, int, int>(2, 3, 1)},
                    {"F", new Tuple<int, int, int>(2, 2, 2)},
                    {"C", new Tuple<int, int, int>(2, 1, 3)}
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
                var scoredPoints = int.Parse(fields[5]);
                var rebounds = int.Parse(fields[6]);
                var assists = int.Parse(fields[7]);

                var points = scoredPoints * positionPoints[position].Item1 +
                             rebounds * positionPoints[position].Item2 +
                             assists * positionPoints[position].Item3;

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
                mvp.RatingPoints += 10;
            }

            mvp.WinningTeam = winner.Team;
            mvp.TeamScore = winner.GoalsMade;

            return mvp;
        }

    }
}

