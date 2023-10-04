using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Arma { 
    public string nombre { get; set;  }
    public int ataque { get; set; }
}

public class Player { 
    public string nombre { get; set; }
    public int vida { get; set; }
    public int ataque { get; set; }
    public int defensa { get; set; }
    public int velocidad { get; set; }
    
    public Arma arma { get; set; }

    public Team team;
    public int getAtaqueTotal()
    {
        return (this.ataque + this.arma.ataque);
    }

    public int getDefensaTotal() {
        return (this.defensa > 95 ? 95 : this.defensa);
    }

    public int TakeDamage(int damage)
    {
        MonoBehaviour.print("ANTES:" + this);
        this.vida = (int)(this.vida - ((damage) * (100 - this.defensa) / 100) * UnityEngine.Random.Range(0.8f, 1.2f));
        MonoBehaviour.print("DESPUES:" + this);
        if (this.IsDead()) {
            this.team.arena.cemetery.Add(this);
            this.team.arena.attackOrder.Remove(this);
            this.team.Remove(this);
            MonoBehaviour.print(this + " se murio!");
        }

        return this.vida;
    }

    public bool IsDead()
    {
        return this.vida <= 0;
    }

    public bool IsAlive() {
    	return this.vida > 0;    
    }

    public override string ToString()
    {
        return this.team.teamName + " --> " + this.nombre + "[vida: " + this.vida + "; vel.:" + this.velocidad + "]";
    }


}

public class Cemetery : List<Player>{ 
}

public class Team : List<Player> {

    public string teamName;
    public Arena arena;

    public Team(string teamName)
    {
        this.teamName = teamName;
    }
    public int AddPlayer(string __nombre, int __vida, int __ataque, int __defensa, Arma __arma, int __velocidad)
    {
        Player player = new Player();
        
        player.nombre = __nombre;
        player.vida = __vida;
        player.ataque = __ataque;
        player.defensa = __defensa;
        player.arma = __arma;
        player.velocidad = __velocidad;
        player.team = this;
        this.Add(player);

        return this.Count;
    }



    public override string ToString()
    {
        return this.teamName;
    }
}

public class Arena : List<Team> 
{
    public List<Player> attackOrder;
    public Cemetery cemetery;

    public Arena() {
        attackOrder = new List<Player>();
        cemetery = new Cemetery();
    }

    public Team AddTeam(string teamName) {
        Team found = this.Find(_team => _team.teamName.Contains(teamName));
        
        Console.WriteLine(found);
        Team team = new Team(teamName);
        team.arena = this;
        this.Add(team);
        return team;
    }

    public void UpdateAttackOrder() {
        if (attackOrder != null) {
            attackOrder.Clear();
        }
        foreach (Team t in this) {
            foreach (Player p in t) {
                attackOrder.Add(p);
            }
        }
        attackOrder.Sort((a, b) => b.velocidad.CompareTo(a.velocidad));
    }

    public Player GetNextAttacker() {
        if (attackOrder == null || !attackOrder.Any()) {
            UpdateAttackOrder();
        }
        Player retval = attackOrder.ElementAt(0);
        attackOrder.RemoveAt(0);
        return retval;
    }

    public Player GetEnemy(Player currentPlayer) {
        Team t = this.Find(x => x.teamName != currentPlayer.team.teamName);
        if (t.Any()) {
            t.Sort((a, b) => a.vida.CompareTo(b.vida));
            return t.Last();
        }
        return null;
    }
    
}



public class Log : MonoBehaviour
{
    public Arena arena;

    void Start()
    {
        arena = new Arena();

        Team teamA = arena.AddTeam("Equipo A");
        Team teamB = arena.AddTeam("Equipo B");

        Arma espaditaComun = new Arma();
        espaditaComun.nombre = "Excalibur";
        espaditaComun.ataque = 30;

        Arma arquitoComun = new Arma();
        arquitoComun.nombre = "Polaris";
        arquitoComun.ataque = 20;

        Arma pistolasLegendarias = new Arma();
        pistolasLegendarias.nombre = "Ebony e Ivory";
        pistolasLegendarias.ataque = 42;

        teamA.AddPlayer("Marcos", 130, 100, 65, espaditaComun, 31);
        teamA.AddPlayer("Pedro", 120, 105, 75, arquitoComun, 36);
        teamA.AddPlayer("Sebastian", 140, 95, 70, pistolasLegendarias, 55);

        teamB.AddPlayer("Santiago", 125, 110, 60, espaditaComun, 29);
        teamB.AddPlayer("Gonzalo", 135, 90, 57, pistolasLegendarias, 56);
        teamB.AddPlayer("Agustín", 125, 95, 78, arquitoComun, 47);


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            AttackFunc2();
        }   
    }

    void AttackFunc2()
    {
        Player currentPlayer = arena.GetNextAttacker();
        Player currentEnemy = arena.GetEnemy(currentPlayer);

        print(currentPlayer + " -- ATACO --> " + currentEnemy);


        if (currentEnemy == null)
        {
            Console.WriteLine("Fin del combate!");
            Console.WriteLine("Ganador del torneo:" + currentPlayer.team.teamName);
        }
        else
        {
            currentEnemy.TakeDamage(currentPlayer.getAtaqueTotal());
        }

    }




}

























